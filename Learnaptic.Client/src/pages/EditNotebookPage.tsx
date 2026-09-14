import type { CreateNotebookRequest } from "@/types/Notebooks/CreateNotebookRequest";
import type { NotebookDetail } from "@/types/Notebooks/NotebookDetail";
import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router";
import { API_URL } from "@/config/api";
import NotebookForm from "@/components/notebooks/NotebookForm";

function EditNotebookPage() {
  const { id } = useParams<{ id: string }>();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [notebook, setNotebook] = useState<NotebookDetail | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    async function loadNotebook() {
      const response = await fetch(`${API_URL}/api/notebooks/${id}`, {
        credentials: "include",
      });

      if (!response.ok) {
        return;
      }

      const data: NotebookDetail = await response.json();
      setNotebook(data);
    }
    loadNotebook();
  }, [id]);

  async function onSubmit(
    notebookRequest: CreateNotebookRequest
  ): Promise<string | null> {
    if (isSubmitting) {
      return null;
    }

    setIsSubmitting(true);

    try {
      const response = await fetch(`${API_URL}/api/notebooks/${id}`, {
        credentials: "include",
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(notebookRequest),
      });

      if (!response.ok) {
        return "Failed to update notebook";
      }

      navigate(`/notebooks/`);
    } catch {
      return "Unable to connect to the server";
    } finally {
      setIsSubmitting(false);
    }
    return null;
  }

  if (notebook === null) {
    return (
      <main className="mx-auto max-w-6xl px-6 py-8">
        <div className="mx-auto max-w-xl">
          <p>Loading notebook...</p>
        </div>
      </main>
    );
  }

  return (
    <main className="mx-auto max-w-6xl px-6 py-8">
      <div className="mx-auto max-w-xl">
        <h1 className="text-xl font-bold">Edit Notebook</h1>
        <NotebookForm
          initialTitle={notebook.title}
          initialSubject={notebook.subject ?? ""}
          initialColor={notebook.color}
          submitLabel="Save"
          isSubmitting={isSubmitting}
          onSubmit={onSubmit}
        />
      </div>
    </main>
  );
}

export default EditNotebookPage;
