import { Navigate, Outlet } from "react-router";
import { useAuth } from "@/AuthContext";

function PublicOnlyRoute() {
  const { user, loading } = useAuth();

  if (loading) {
    return null;
  }

  if (user) {
    return <Navigate to="/notebooks" replace />;
  }

  return <Outlet />;
}

export default PublicOnlyRoute;
