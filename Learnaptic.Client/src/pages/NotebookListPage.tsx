import { type NotebookColor } from "@/constants/notebookColors";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { API_URL } from "@/config/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
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
  const [search, setSearch] = useState("");

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
      previousNotebooks.filter((notebook) => notebook.id !== idToDelete)
    );
  }

  const searchTerm = search.toLowerCase();
  const filteredNotebooks = notebooks.filter((notebook) => {
    return (
      notebook.title.toLowerCase().includes(searchTerm) ||
      notebook.subject?.toLowerCase().includes(searchTerm)
    );
  });

  return (
    <>
      <main className="mx-auto max-w-6xl px-6 py-8">
        <div className="flex justify-between items-center">
          <div>
            <h1 className="font-bold">Notebooks</h1>
            <p className="text-sm text-muted-foreground">
              Your study materials, all in one place.
            </p>
          </div>
          <Button variant="default" onClick={() => navigate("/notebooks/new")}>
            Create Notebook
          </Button>
        </div>
        <Input
          className="my-4 w-full max-w-lg"
          id="title"
          type="text"
          placeholder="Search"
          value={search}
          onChange={(s) => setSearch(s.target.value)}
        />
        <div className="grid gap-4 md:grid-cols-2">
          {filteredNotebooks.map((notebook) => (
            <NotebookCard
              key={notebook.id}
              id={notebook.id}
              title={notebook.title}
              subject={notebook.subject}
              color={notebook.color}
              lastAccessedAt={notebook.lastAccessedAt}
              onDelete={() => handleDeleteNotebook(notebook.id)}
              onClick={() =>
                navigate(
                  `/notebooks/${notebook.id}/${createSlug(notebook.title)}`
                )
              }
              onEdit={() =>
                navigate(
                  `/notebooks/${notebook.id}/${createSlug(notebook.title)}/edit`
                )
              }
            />
          ))}
        </div>
      </main>
    </>
  );
}

export default NotebookListPage;
