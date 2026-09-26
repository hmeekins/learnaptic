import type { NotebookDetail } from "@/types/Notebooks/NotebookDetail";
import type { Concept } from "@/types/Concepts/Concept";
import { useState, useEffect } from "react";
import { useParams } from "react-router";
import { API_URL } from "@/config/api";
import { Button } from "@/components/ui/button";
import ConceptTableOfContents from "@/components/notebooks/ConceptTableOfContents";
import ConceptEditor from "@/components/notebooks/ConceptEditor";
import useConceptAutosave from "@/hooks/useConceptAutosave";

function NotebookPage() {
  const { id } = useParams<{ id: string }>();
  const [notebook, setNotebook] = useState<NotebookDetail | null>(null);
  const [selectedConceptId, setSelectedConceptId] = useState<number | null>(
    null
  );
  const { handleConceptContentChange, flushPendingSave } = useConceptAutosave({
    id,
    selectedConceptId,
    notebook,
    setNotebook,
  });

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

      if (data.concepts.length > 0) {
        setSelectedConceptId(data.concepts[0].id);
      }
    }
    loadNotebook();
  }, [id]);

  async function handleAddConcept() {
    const saved = await flushPendingSave();

    if (!saved) {
      return;
    }

    const response = await fetch(`${API_URL}/api/notebooks/${id}/concepts`, {
      credentials: "include",
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        title: "Untitled Concept",
      }),
    });

    if (!response.ok) {
      return;
    }

    const newConcept: Concept = await response.json();
    setSelectedConceptId(newConcept.id);

    setNotebook((currentNotebook) => {
      if (currentNotebook === null) {
        return null;
      }

      return {
        ...currentNotebook,
        concepts: [...currentNotebook.concepts, newConcept],
      };
    });
  }

  if (notebook === null) {
    return (
      <main className="mx-auto max-w-4xl px-6 py-8">
        <div className="mx-auto max-w-xl">
          <p>Loading notebook...</p>
        </div>
      </main>
    );
  }

  const selectedConcept = notebook.concepts.find(
    (concept) => concept.id === selectedConceptId
  );

  async function handleSelectConcept(conceptId: number) {
    const saved = await flushPendingSave();

    if (!saved) {
      return;
    }

    setSelectedConceptId(conceptId);
  }

  return (
    <main className="grid grid-cols-5 w-full mx-auto px-6 py-8">
      <div className="col-span-1">
        <ConceptTableOfContents
          concepts={notebook.concepts}
          selectedConceptId={selectedConceptId}
          onSelectConcept={handleSelectConcept}
        />
      </div>

      <div className="col-span-3 px-8">
        <div className="flex justify-between items-center mb-5">
          <div>
            <h1 className="text-xl font-bold">{notebook.title || ""}</h1>
            <p className="text-sm text-muted-foreground">
              {notebook?.subject || ""}
            </p>
          </div>
          <Button variant="default" onClick={handleAddConcept}>
            Add Concept
          </Button>
        </div>

        <section>
          {selectedConcept && (
            <ConceptEditor
              key={selectedConcept.id}
              content={selectedConcept.content}
              onChange={handleConceptContentChange}
            />
          )}
        </section>
      </div>
    </main>
  );
}

export default NotebookPage;
