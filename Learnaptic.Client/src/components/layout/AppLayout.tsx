import { Outlet } from "react-router";
import AppHeader from "@/components/layout/AppHeader";

function AppLayout() {
  return (
    <div className="min-h-screen">
      <AppHeader />
      <main>
        <Outlet />
      </main>
    </div>
  );
}

export default AppLayout;
