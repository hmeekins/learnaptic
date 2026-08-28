import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { useAuth } from "@/AuthContext";
import { NavLink } from "react-router";
import fullLogo from "@/assets/branding/learnaptic-logo.svg";
import iconLogo from "@/assets/branding/learnaptic-logo-small.svg";

function AppHeader() {
  const { user, logout } = useAuth();

  return (
    <header className="border-b">
      <div className="flex items-center justify-between  px-4 py-4">
        <div className="flex items-center gap-6">
          <NavLink to="/study-guides" className="flex items-center">
            <img
              src={fullLogo}
              alt="Learnaptic"
              className="hidden h-12 w-auto sm:block"
            />

            <img
              src={iconLogo}
              alt="Learnaptic"
              className="h-12 w-auto sm:hidden"
            />
          </NavLink>
          <nav className="hidden items-center gap-6 sm:flex">
            <NavLink
              to="/notebooks"
              className={({ isActive }) =>
                isActive
                  ? "font-medium text-primary"
                  : "text-foreground hover:text-primary"
              }
            >
              Notebooks
            </NavLink>

            <NavLink
              to="/study-sets"
              className={({ isActive }) =>
                isActive
                  ? "font-medium text-primary"
                  : "text-foreground hover:text-primary"
              }
            >
              Study Sets
            </NavLink>
          </nav>
        </div>

        <DropdownMenu>
          <DropdownMenuTrigger render={<Button variant="ghost" />}>
            {user?.userName}
          </DropdownMenuTrigger>

          <DropdownMenuContent>
            <DropdownMenuItem disabled>Account</DropdownMenuItem>

            <DropdownMenuItem onClick={logout}>Logout</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>

      <nav className="flex gap-6 px-4 pb-3 sm:hidden">
        <NavLink
          to="/notebooks"
          className={({ isActive }) =>
            isActive
              ? "font-medium text-primary"
              : "text-foreground hover:text-primary"
          }
        >
          Notebooks
        </NavLink>

        <NavLink
          to="/study-sets"
          className={({ isActive }) =>
            isActive
              ? "font-medium text-primary"
              : "text-foreground hover:text-primary"
          }
        >
          Study Sets
        </NavLink>
      </nav>
    </header>
  );
}

export default AppHeader;
