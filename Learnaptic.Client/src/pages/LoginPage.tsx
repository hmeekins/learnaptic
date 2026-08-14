import { useState } from "react";
import { useNavigate } from "react-router";
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
    <main className="min-h-screen md:grid md:grid-cols-10 bg-muted/30">
      <section className="hidden md:flex col-span-6 md:flex-col md:justify-center md:px-24">
        <h1 className="text-4xl font-bold">Learnaptic</h1>
        <p className="mt-4 max-w-md text-muted-foreground">
          Turn your notes into knowledge that sticks.
        </p>
        <div className="mt-8 grid grid-cols-3 gap-3">
          <div className="rounded-lg border p-4">
            <h2 className="font-medium">Organize</h2>
            <p className="mt-2 text-sm text-muted-foreground">
              Keep your learning material structured and easy to revisit.
            </p>
          </div>
          <div className="rounded-lg border p-4">
            <h2 className="font-medium">Practice</h2>
            <p className="mt-2 text-sm text-muted-foreground">
              Test your knowledge with interactive quizzes and flashcards.
            </p>
          </div>
          <div className="rounded-lg border p-4">
            <h2 className="font-medium">Understand</h2>
            <p className="mt-2 text-sm text-muted-foreground">
              Break complex topics into manageable pieces for better
              comprehension.
            </p>
          </div>
        </div>
      </section>
      <section className="col-span-4 flex items-center justify-center px-4">
        <Card className="w-full max-w-lg shadow-lg">
          <CardHeader>
            <CardTitle>Welcome back</CardTitle>
            <CardDescription>Log in to continue to Learnaptic.</CardDescription>
          </CardHeader>

          <CardContent>
            <form onSubmit={handleSubmit} className="space-y-6">
              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <Input
                  id="email"
                  type="email"
                  placeholder="you@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
              </div>

              <div className="space-y-2">
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
