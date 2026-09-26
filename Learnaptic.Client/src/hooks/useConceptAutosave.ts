import type { Dispatch, SetStateAction } from "react";
import type { JSONContent } from "@tiptap/react";
import type { NotebookDetail } from "@/types/Notebooks/NotebookDetail";
import { API_URL } from "@/config/api";
import { useRef } from "react";

interface UseConceptAutosaveProps {
  id: string | undefined;
  selectedConceptId: number | null;
  notebook: NotebookDetail | null;
  setNotebook: Dispatch<SetStateAction<NotebookDetail | null>>;
}

function useConceptAutosave({
  id,
  selectedConceptId,
  notebook,
  setNotebook,
}: UseConceptAutosaveProps) {
  const saveTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const pendingSaveRef = useRef<{
    conceptId: number;
    content: JSONContent;
  } | null>(null);

  function handleConceptContentChange(content: JSONContent) {
    if (selectedConceptId === null) {
      return;
    }

    if (saveTimeoutRef.current) {
      clearTimeout(saveTimeoutRef.current);
    }

    pendingSaveRef.current = {
      conceptId: selectedConceptId,
      content,
    };

    saveTimeoutRef.current = setTimeout(async () => {
      saveTimeoutRef.current = null;
      await flushPendingSave();
    }, 750);
  }

  async function saveConcept(
    conceptId: number,
    content: JSONContent
  ): Promise<boolean> {
    if (!id) {
      return false;
    }

    const concept = notebook?.concepts.find(
      (concept) => concept.id === conceptId
    );

    if (!concept) {
      return false;
    }

    const response = await fetch(
      `${API_URL}/api/notebooks/${id}/concepts/${conceptId}`,
      {
        method: "PUT",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          title: concept.title,
          content,
        }),
      }
    );

    if (!response.ok) {
      console.error("Failed to save concept");
      return false;
    }

    setNotebook((currentNotebook) => {
      if (!currentNotebook) {
        return null;
      }

      return {
        ...currentNotebook,
        concepts: currentNotebook.concepts.map((concept) =>
          concept.id === conceptId ? { ...concept, content } : concept
        ),
      };
    });

    return true;
  }

  async function flushPendingSave(): Promise<boolean> {
    const pendingSave = pendingSaveRef.current;

    if (!pendingSave) {
      return true;
    }

    if (saveTimeoutRef.current) {
      clearTimeout(saveTimeoutRef.current);
      saveTimeoutRef.current = null;
    }

    const saved = await saveConcept(pendingSave.conceptId, pendingSave.content);

    if (saved && pendingSaveRef.current === pendingSave) {
      pendingSaveRef.current = null;
    }

    return saved;
  }

  return {
    handleConceptContentChange,
    flushPendingSave,
  };
}

export default useConceptAutosave;
