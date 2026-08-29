import { useState, useEffect } from "react";
import { useParams } from "react-router";
import { API_URL } from "@/config/api";
import ConceptTableOfContents from "@/components/notebooks/ConceptTableOfContents";
import ConceptCard from "@/components/notebooks/ConceptCard";
import type { NotebookDetail } from "@/types/Notebooks/NotebookDetail";
import type { Concept } from "@/types/Concepts/Concept";

function NotebookPage() {
  const { id } = useParams<{ id: string }>();
  const [notebook, setNotebook] = useState<NotebookDetail | null>(null);
  useEffect(() => {
    async function loadNotebook() {
      const response = await fetch(`${API_URL}/api/notebooks/${id}`);
      const data: NotebookDetail = await response.json();
      setNotebook(data);
    }
    loadNotebook();
  }, []);

  if (notebook === null) {
    return (
      <>
        <main>
          <p>Loading study guide...</p>
        </main>
      </>
    );
  }
  return (
    <>
      <main>
        <header>
          <h1>{notebook?.title || ""}</h1>
          <p>{notebook?.subject || ""}</p>
        </header>

        <ConceptTableOfContents concepts={notebook?.concepts || []} />

        <section>
          {notebook?.concepts.map((concept: Concept) => (
            <ConceptCard key={concept.id} concept={concept} />
          ))}
        </section>
      </main>
    </>
  );
}

export default NotebookPage;
