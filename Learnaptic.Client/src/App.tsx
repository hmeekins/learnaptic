import { createBrowserRouter, Navigate, RouterProvider } from "react-router";
import * as pages from "./pages";

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

function App() {
  return <RouterProvider router={router} />;
}

export default App;
