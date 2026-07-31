import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router'
import StudyGuideListPage from './pages/StudyGuideListPage'
import CreateStudyGuidePage from './pages/CreateStudyGuidePage'
import './index.css'

const router = createBrowserRouter([
  {
    path: '/study-guides',
    Component: StudyGuideListPage
  },
  {
    path: '/study-guides/new',
    Component: CreateStudyGuidePage
  }
])

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>,
)
