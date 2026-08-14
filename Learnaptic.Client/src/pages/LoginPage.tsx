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
    <main className="min-h-screen md:grid md:grid-cols-5">
      {/* Left side */}
      <section className="hidden bg-muted/70 md:col-span-3 md:flex md:items-center md:justify-center">
        <div className="mx-auto w-full max-w-3xl px-8 lg:max-w-4xl lg:px-12 xl:max-w-5xl xl:px-16">
          <h1 className="text-4xl font-bold text-primary lg:text-5xl">
            Learnaptic
          </h1>

          <p className="mt-4 text-lg text-muted-foreground lg:text-xl">
            Turn your notes into knowledge that sticks.
          </p>

          <p className="mt-3 max-w-3xl text-muted-foreground lg:text-lg">
            Learnaptic brings your study materials together in one focused
            workspace. Build organized notebooks, turn what you&apos;re learning
            into flashcards and practice questions, and use intelligent study
            tools to understand, review, and retain difficult material.
          </p>

          <div className="mt-8 grid w-full gap-3 lg:gap-4">
            <div className="rounded-lg border bg-card p-4 lg:p-5">
              <h2 className="font-medium lg:text-lg">Organize</h2>

              <p className="mt-2 text-sm text-muted-foreground lg:text-base">
                Keep your learning material structured and easy to revisit.
              </p>
            </div>

            <div className="rounded-lg border bg-card p-4 lg:p-5">
              <h2 className="font-medium lg:text-lg">Practice</h2>

              <p className="mt-2 text-sm text-muted-foreground lg:text-base">
                Test your knowledge with interactive quizzes and flashcards.
              </p>
            </div>

            <div className="rounded-lg border bg-card p-4 lg:p-5">
              <h2 className="font-medium lg:text-lg">Understand</h2>

              <p className="mt-2 text-sm text-muted-foreground lg:text-base">
                Break complex topics into manageable pieces for better
                comprehension.
              </p>
            </div>
          </div>
        </div>
      </section>

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
