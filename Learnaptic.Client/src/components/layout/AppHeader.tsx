import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { useAuth } from "@/AuthContext";
import { Link } from "react-router";
import learnapticLogo from "@/assets/branding/learnaptic-logo.svg";

function AppHeader() {
  const { user, logout } = useAuth();

  return (
    <header className="flex items-center justify-between border-b px-6 py-4">
      <div className="flex items-center gap-8">
        <Link to="/study-guides" className="flex items-center gap-2">
          <img src={learnapticLogo} alt="" className="h-12 w-auto" />
          <span className="font-bold text-primary text-2xl">Learnaptic</span>
        </Link>

        <nav className="flex items-center gap-6">
          <Link to="/notebooks">Notebooks</Link>
          <Link to="/study-sets">Study Sets</Link>
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
    </header>
  );
}

export default AppHeader;
