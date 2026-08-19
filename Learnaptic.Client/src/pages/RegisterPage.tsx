import { useState } from "react";
import { useNavigate } from "react-router";
import { API_URL } from "@/config/api";
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
import AuthInfoPanel from "@/components/auth/AuthInfoPanel";
import type { ApiErrorResponse } from "@/types/Errors/ApiErrorResponse";

function RegisterPage() {
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const hasMinimumLength = password.length >= 10;
  const hasUppercase = /[A-Z]/.test(password);
  const hasLowercase = /[a-z]/.test(password);
  const hasNumber = /[0-9]/.test(password);
  const hasSpecialCharacter = /[^a-zA-Z0-9]/.test(password);

  const navigate = useNavigate();

  const isFormValid = () => {
    if (!email || !username || !password || !confirmPassword) {
      setError("Please fill in all fields.");
      return false;
    }

    if (password != confirmPassword) {
      setError("Passwords do not match");
      return false;
    }

    if (
      password.length < 10 ||
      !/[A-Z]/.test(password) ||
      !/[a-z]/.test(password) ||
      !/[0-9]/.test(password) ||
      !/[^a-zA-Z0-9]/.test(password)
    ) {
      setError("Password does not meet requirements");
      return false;
    }

    if (username.length < 3) {
      setError("Username must be at least 3 characters.");
      return false;
    }

    return true;
  };

  const handleApiError = (data: ApiErrorResponse) => {
    const duplicateUsername = data.errors.some(
      (error) => error.code === "DuplicateUserName"
    );

    const duplicateEmail = data.errors.some(
      (error) => error.code === "DuplicateEmail"
    );

    if (duplicateUsername) {
      setError("That username is already taken.");
    } else if (duplicateEmail) {
      setError("An account with that email already exists.");
    } else {
      setError(data.message);
    }
  };

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!isFormValid()) {
      return;
    }

    setError("");
    setLoading(true);

    try {
      const response = await fetch(`${API_URL}/api/auth/register`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify({ email, username, password }),
      });

      if (!response.ok) {
        const data: ApiErrorResponse = await response.json();
        handleApiError(data);
        return;
      }

      navigate("/login");
    } catch {
      setError("An error occurred while trying to register. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="min-h-screen md:grid md:grid-cols-5">
      {/* Left side */}
      <AuthInfoPanel />

      {/* Register side */}
      <section className="flex min-h-screen items-center justify-center px-4 md:col-span-2 md:px-8 lg:px-10 xl:px-12">
        <Card className="w-full max-w-md border lg:max-w-lg xl:max-w-xl">
          <CardHeader className="lg:px-8 lg:pt-8">
            <CardTitle className="lg:text-xl">Create Your Account</CardTitle>

            <CardDescription className="lg:text-base">
              Start organizing and studying with Learnaptic
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
                <Label htmlFor="username">Username</Label>

                <Input
                  id="username"
                  type="text"
                  placeholder="Username"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
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

              <div className="space-y-3">
                <Label htmlFor="confirmPassword">Confirm Password</Label>

                <Input
                  id="confirmPassword"
                  type="password"
                  placeholder="Please confirm your password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
                <div className="space-y-1 text-sm text-muted-foreground">
                  <p>Password must contain:</p>
                  <ul className="list-disc pl-5">
                    <li
                      className={
                        hasMinimumLength
                          ? "text-primary"
                          : "text-muted-foreground"
                      }
                    >
                      At least 10 characters
                    </li>
                    <li
                      className={
                        hasUppercase ? "text-primary" : "text-muted-foreground"
                      }
                    >
                      One uppercase letter
                    </li>
                    <li
                      className={
                        hasLowercase ? "text-primary" : "text-muted-foreground"
                      }
                    >
                      One lowercase letter
                    </li>
                    <li
                      className={
                        hasNumber ? "text-primary" : "text-muted-foreground"
                      }
                    >
                      One number
                    </li>
                    <li
                      className={
                        hasSpecialCharacter
                          ? "text-primary"
                          : "text-muted-foreground"
                      }
                    >
                      One special character
                    </li>
                  </ul>
                </div>
              </div>

              {error && <p className="text-sm text-destructive">{error}</p>}

              <Button type="submit" className="w-full" disabled={loading}>
                {loading ? "Creating account..." : "Create Account"}
              </Button>

              <div className="text-center text-sm">
                <span className="text-muted-foreground">
                  Already have an account?
                </span>

                <Button
                  type="button"
                  variant="link"
                  onClick={() => navigate("/login")}
                >
                  Login
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      </section>
    </main>
  );
}

export default RegisterPage;
