import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import NavBar from "../components/NavBar";
import StudyGuideCard from "../components/StudyGuideCard";

type StudyGuideListItem = {
  id: number;
  title: string;
  subject?: string;
  lastAccessedAt: string;
};

function StudyGuideListPage() {
  const navigate = useNavigate();
  const [studyGuides, setStudyGuides] = useState<StudyGuideListItem[]>([]);

  useEffect(() => {
    async function loadStudyGuides() {
      const response = await fetch("https://localhost:7057/api/study-guides");
      const data = await response.json();

      setStudyGuides(data);
    }

    loadStudyGuides();
  }, []);

  async function handleDeleteStudyGuide(idToDelete: number) {
    const response = await fetch(
      `https://localhost:7057/api/study-guides/${idToDelete}`,
      { method: "DELETE" }
    );

    if (!response.ok) {
      throw new Error(`Failed to delete study guide: ${response.status}`);
    }

    setStudyGuides((previousStudyGuides) =>
      previousStudyGuides.filter((guide) => guide.id !== idToDelete)
    );
  }

  return (
    <>
      <NavBar />
      <main>
        <h1>Study Guides</h1>
        <p>Your study materials, all in one place.</p>
        {studyGuides.map((guide) => (
          <StudyGuideCard
            key={guide.id}
            id={guide.id}
            title={guide.title}
            subject={guide.subject}
            lastAccessedAt={guide.lastAccessedAt}
            onDelete={() => handleDeleteStudyGuide(guide.id)}
            onClick={() => navigate(`/study-guides/${guide.id}/${guide.title}`)}
          />
        ))}
        <button onClick={() => navigate("/study-guides/new")}>
          Create New Study Guide
        </button>
      </main>
    </>
  );
}

export default StudyGuideListPage;
