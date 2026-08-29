import { type NotebookColor } from "@/constants/notebookColors";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { API_URL } from "@/config/api";
import createSlug from "@/utils/createSlug";
import NotebookCard from "@/components/notebooks/NotebookCard";

type NotebookListItem = {
  id: number;
  title: string;
  color: NotebookColor;
  subject?: string;
  lastAccessedAt: string;
};

function NotebookListPage() {
  const navigate = useNavigate();
  const [notebooks, setNotebooks] = useState<NotebookListItem[]>([]);

  useEffect(() => {
    async function loadNotebooks() {
      const response = await fetch(`${API_URL}/api/notebooks`, {
        credentials: "include",
      });

      if (!response.ok) {
        throw new Error(`Failed to load notebooks: ${response.status}`);
      }

      const data = await response.json();
      setNotebooks(data);
    }

    loadNotebooks();
  }, []);

  async function handleDeleteNotebook(idToDelete: number) {
    const response = await fetch(`${API_URL}/api/notebooks/${idToDelete}`, {
      credentials: "include",
      method: "DELETE",
    });

    if (!response.ok) {
      throw new Error(`Failed to delete notebook: ${response.status}`);
    }

    setNotebooks((previousNotebooks) =>
      previousNotebooks.filter((guide) => guide.id !== idToDelete)
    );
  }

  return (
    <>
      <main>
        <h1>Notebooks</h1>
        <p>Your study materials, all in one place.</p>
        {notebooks.map((guide) => (
          <NotebookCard
            key={guide.id}
            id={guide.id}
            title={guide.title}
            subject={guide.subject}
            color={guide.color}
            lastAccessedAt={guide.lastAccessedAt}
            onDelete={() => handleDeleteNotebook(guide.id)}
            onClick={() =>
              navigate(`/notebooks/${guide.id}/${createSlug(guide.title)}`)
            }
          />
        ))}
        <button onClick={() => navigate("/notebooks/new")}>
          Create New Notebook
        </button>
      </main>
    </>
  );
}

export default NotebookListPage;
