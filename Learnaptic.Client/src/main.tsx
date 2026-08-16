import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, Navigate, RouterProvider } from "react-router";
import * as pages from "./pages";
import "./index.css";

const router = createBrowserRouter([
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
]);

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);
