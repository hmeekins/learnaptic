import { useState, useEffect } from "react";
import { useParams } from "react-router";
import { API_URL } from "@/config/api";
import ConceptTableOfContents from "../components/notebooks/ConceptTableOfContents";
import ConceptCard from "../components/notebooks/ConceptCard";
import type { StudyGuideDetail } from "../types/StudyGuides/StudyGuideDetail";
import type { Concept } from "../types/Concepts/Concept";

function StudyGuidePage() {
  const { id } = useParams<{ id: string }>();
  const [studyGuide, setStudyGuide] = useState<StudyGuideDetail | null>(null);
  useEffect(() => {
    async function loadStudyGuide() {
      const response = await fetch(`${API_URL}/api/study-guides/${id}`);
      const data: StudyGuideDetail = await response.json();
      setStudyGuide(data);
    }
    loadStudyGuide();
  }, []);

  if (studyGuide === null) {
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
          <h1>{studyGuide?.title || ""}</h1>
          <p>{studyGuide?.subject || ""}</p>
        </header>

        <ConceptTableOfContents concepts={studyGuide?.concepts || []} />

        <section>
          {studyGuide?.concepts.map((concept: Concept) => (
            <ConceptCard key={concept.id} concept={concept} />
          ))}
        </section>
      </main>
    </>
  );
}

export default StudyGuidePage;
