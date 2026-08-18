import { useState, useEffect } from "react";
import { useParams } from "react-router";
import type { StudyGuideDetail } from "../types/StudyGuides/StudyGuideDetail";
import type { Concept } from "../types/Concepts/Concept";
import NavBar from "../components/layout/NavBar";
import ConceptTableOfContents from "../components/notebooks/ConceptTableOfContents";
import ConceptCard from "../components/ConceptCard";

function StudyGuidePage() {
  const { id } = useParams<{ id: string }>();
  const [studyGuide, setStudyGuide] = useState<StudyGuideDetail | null>(null);
  useEffect(() => {
    async function loadStudyGuide() {
      const response = await fetch(
        `https://localhost:7057/api/study-guides/${id}`
      );
      const data: StudyGuideDetail = await response.json();
      setStudyGuide(data);
    }
    loadStudyGuide();
  }, []);

  if (studyGuide === null) {
    return (
      <>
        <NavBar />
        <main>
          <p>Loading study guide...</p>
        </main>
      </>
    );
  }
  return (
    <>
      <NavBar />
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
