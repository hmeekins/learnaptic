import type { NotebookDetail } from "@/types/Notebooks/NotebookDetail";
import type { Concept } from "@/types/Concepts/Concept";
import { useState, useEffect } from "react";
import { useParams } from "react-router";
import { API_URL } from "@/config/api";
import { Button } from "@/components/ui/button";
import ConceptTableOfContents from "@/components/notebooks/ConceptTableOfContents";
import ConceptCard from "@/components/notebooks/ConceptCard";

function NotebookPage() {
  const { id } = useParams<{ id: string }>();
  const [notebook, setNotebook] = useState<NotebookDetail | null>(null);
  useEffect(() => {
    async function loadNotebook() {
      const response = await fetch(`${API_URL}/api/notebooks/${id}`, {
        credentials: "include",
      });
      const data: NotebookDetail = await response.json();
      setNotebook(data);
    }
    loadNotebook();
  }, []);

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
    <main className="mx-auto max-w-4xl px-6 py-8">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-xl font-bold">{notebook.title || ""}</h1>
          <p className="text-sm text-muted-foreground">
            {notebook?.subject || ""}
          </p>
        </div>
        <Button variant="default">Add Concept</Button>
      </div>
      <ConceptTableOfContents concepts={notebook.concepts || []} />

      <section>
        {notebook?.concepts.map((concept: Concept) => (
          <ConceptCard key={concept.id} concept={concept} />
        ))}
      </section>
    </main>
  );
}

export default NotebookPage;
