import { useState } from "react";
import { useNavigate } from "react-router";
import NavBar from "../components/NavBar";
import TextInput from "../components/TextInput";
import type { CreateStudyGuideRequest } from "../types/StudyGuides/CreateStudyGuideRequest";
import type { StudyGuideDetail } from "../types/StudyGuides/StudyGuideDetail";

function CreateStudyGuidePage() {
  const [title, setTitle] = useState("");
  const [subject, setSubject] = useState("");

  const navigate = useNavigate();

  async function handleSubmit(event: React.SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
    if (title.trim() === "") {
      return;
    }

    const studyGuideRequest: CreateStudyGuideRequest = {
      title: title.trim(),
    };
    if (subject.trim() !== "") {
      studyGuideRequest.subject = subject.trim();
    }
    const response = await fetch("https://localhost:7057/api/study-guides", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(studyGuideRequest),
    });

    if (!response.ok) {
      throw new Error(`Failed to create study guide: ${response.status}`);
    }

    const responseData: StudyGuideDetail = await response.json();
    navigate(`/study-guides/${responseData.id}/${responseData.title}`);
  }

  return (
    <>
      <NavBar />
      <main>
        <h1>Create Study Guide</h1>
        <p>Create a new study guide to organize your learning.</p>

        <form onSubmit={handleSubmit}>
          <TextInput
            id="title"
            label="Title"
            value={title}
            onChange={setTitle}
          />

          <TextInput
            id="subject"
            label="Subject"
            value={subject}
            onChange={setSubject}
          />

          <button type="submit">Create Study Guide</button>
        </form>
      </main>
    </>
  );
}

export default CreateStudyGuidePage;
