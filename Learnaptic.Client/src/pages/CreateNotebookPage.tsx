import type { CreateNotebookRequest } from "@/types/Notebooks/CreateNotebookRequest";
import type { NotebookDetail } from "@/types/Notebooks/NotebookDetail";
import { useState } from "react";
import { useNavigate } from "react-router";
import { API_URL } from "@/config/api";
import NotebookForm from "@/components/notebooks/NotebookForm";
import createSlug from "@/utils/createSlug";

function CreateNotebookPage() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();

  async function onSubmit(
    notebookRequest: CreateNotebookRequest
  ): Promise<string | null> {
    if (isSubmitting) {
      return null;
    }

    setIsSubmitting(true);

    try {
      const response = await fetch(`${API_URL}/api/notebooks`, {
        credentials: "include",
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(notebookRequest),
      });

      if (!response.ok) {
        return "Failed to create notebook";
      }

      const responseData: NotebookDetail = await response.json();

      navigate(
        `/notebooks/${responseData.id}/${createSlug(responseData.title)}`
      );
    } catch {
      return "Unable to connect to the server";
    } finally {
      setIsSubmitting(false);
    }
    return null;
  }

  return (
    <main className="mx-auto max-w-6xl px-6 py-8">
      <div className="mx-auto max-w-xl">
        <div className="space-y-1">
          <h1 className="text-xl font-bold">Create Notebook</h1>

          <p className="text-sm text-muted-foreground">
            Create a new notebook to organize your learning.
          </p>
        </div>
        <NotebookForm
          submitLabel="Create Notebook"
          isSubmitting={isSubmitting}
          onSubmit={onSubmit}
        />
      </div>
    </main>
  );
}

export default CreateNotebookPage;
