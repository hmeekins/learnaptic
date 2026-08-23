import { createBrowserRouter, Navigate, RouterProvider } from "react-router";
import ProtectedRoute from "@/components/auth/ProtectedRoute";
import PublicOnlyRoute from "@/components/auth/PublicOnlyRoute";
import AppLayout from "@/components/layout/AppLayout";
import * as pages from "@/pages";

const router = createBrowserRouter([
  {
    Component: PublicOnlyRoute,
    children: [
      {
        path: "/",
        element: <Navigate to="/login" replace />,
      },
      {
        path: "/login",
        Component: pages.LoginPage,
      },
      {
        path: "/register",
        Component: pages.RegisterPage,
      },
    ],
  },
  {
    Component: ProtectedRoute,
    children: [
      {
        Component: AppLayout,
        children: [
          {
            path: "/study-guides",
            Component: pages.StudyGuideListPage,
          },
          {
            path: "/study-guides/new",
            Component: pages.CreateStudyGuidePage,
          },
          {
            path: "/study-guides/:id/:slug",
            Component: pages.StudyGuidePage,
          },
        ],
      },
    ],
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
