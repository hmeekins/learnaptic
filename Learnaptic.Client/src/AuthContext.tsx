import {
  createContext,
  useContext,
  useState,
  useEffect,
  type ReactNode,
} from "react";
import type { CurrentUser } from "@/types/User/CurrentUser";

interface AuthProviderProps {
  children: ReactNode;
}

interface AuthContextValue {
  user: CurrentUser | null;
  loading: boolean;
  refreshUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<CurrentUser | null>(null);
  const [loading, setLoading] = useState(true);

  const refreshUser = async () => {
    try {
      const response = await fetch("https://localhost:7057/api/auth/me", {
        credentials: "include",
      });

      if (!response.ok) {
        setUser(null);
        return;
      }

      const data: CurrentUser = await response.json();
      setUser(data);
    } catch {
      setUser(null);
    }
  };

  useEffect(() => {
    const InitializeAuth = async () => {
      await refreshUser();
      setLoading(false);
    };

    InitializeAuth();
  }, []);

  return (
    <AuthContext.Provider value={{ user, loading, refreshUser }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (context === undefined) {
    throw new Error("useAuth must be used within an AuthProvider");
  }

  return context;
}
