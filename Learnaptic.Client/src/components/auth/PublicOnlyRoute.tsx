import { Navigate, Outlet } from "react-router";
import { useAuth } from "@/AuthContext";

function PublicOnlyRoute() {
  const { user, loading } = useAuth();

  if (loading) {
    return null;
  }

  if (user) {
    return <Navigate to="/study-guides" replace />;
  }

  return <Outlet />;
}

export default PublicOnlyRoute;
