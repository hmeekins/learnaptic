import { useState } from "react";
import { useNavigate } from "react-router";
import AuthInfoPanel from "@/components/auth/AuthInfoPanel";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const response = await fetch("https://localhost:7057/api/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({ email, password }),
      });

      if (!response.ok) {
        setError("Invalid email or password");
        return;
      }

      navigate("/study-guides");
    } catch {
      setError("An error occurred while trying to log in. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="min-h-screen md:grid md:grid-cols-5">
      {/* Left side */}
      <AuthInfoPanel />

      {/* Login side */}
      <section className="flex min-h-screen items-center justify-center px-4 md:col-span-2 md:px-8 lg:px-10 xl:px-12">
        <Card className="w-full max-w-md border lg:max-w-lg xl:max-w-xl">
          <CardHeader className="lg:px-8 lg:pt-8">
            <CardTitle className="lg:text-xl">Welcome back</CardTitle>

            <CardDescription className="lg:text-base">
              Log in to continue to Learnaptic.
            </CardDescription>
          </CardHeader>

          <CardContent className="lg:px-8 lg:pb-8">
            <form onSubmit={handleSubmit} className="space-y-8">
              <div className="space-y-3">
                <Label htmlFor="email">Email</Label>

                <Input
                  id="email"
                  type="email"
                  placeholder="you@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>

              <div className="space-y-3">
                <Label htmlFor="password">Password</Label>

                <Input
                  id="password"
                  type="password"
                  placeholder="Enter your password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </div>

              {error && <p className="text-sm text-destructive">{error}</p>}

              <Button type="submit" className="w-full" disabled={loading}>
                {loading ? "Logging in..." : "Login"}
              </Button>

              <div className="text-center text-sm">
                <span className="text-muted-foreground">
                  Don&apos;t have an account?
                </span>

                <Button
                  type="button"
                  variant="link"
                  onClick={() => navigate("/register")}
                >
                  Create an account
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      </section>
    </main>
  );
}

export default LoginPage;
