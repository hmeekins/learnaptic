import { useState } from "react";
import { useNavigate } from "react-router";
import { API_URL } from "@/config/api";
import TextInput from "../components/TextInput";
import type { CreateNotebookRequest } from "../types/Notebooks/CreateNotebookRequest";
import type { NotebookDetail } from "../types/Notebooks/NotebookDetail";

function CreateNotebookPage() {
  const [title, setTitle] = useState("");
  const [subject, setSubject] = useState("");

  const navigate = useNavigate();

  async function handleSubmit(event: React.SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
    if (title.trim() === "") {
      return;
    }

    const notebookRequest: CreateNotebookRequest = {
      title: title.trim(),
    };
    if (subject.trim() !== "") {
      notebookRequest.subject = subject.trim();
    }
    const response = await fetch(`${API_URL}/api/notebooks`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(notebookRequest),
    });

    if (!response.ok) {
      throw new Error(`Failed to create notebook: ${response.status}`);
    }

    const responseData: NotebookDetail = await response.json();
    navigate(`/notebooks/${responseData.id}/${responseData.title}`);
  }

  return (
    <>
      <main>
        <h1>Create Notebook</h1>
        <p>Create a new notebook to organize your learning.</p>

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

          <button type="submit">Create Notebook</button>
        </form>
      </main>
    </>
  );
}

export default CreateNotebookPage;
