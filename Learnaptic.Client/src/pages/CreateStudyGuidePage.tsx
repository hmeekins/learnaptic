import { useState } from "react";
import NavBar from "../components/NavBar";
import TextInput from "../components/TextInput";

function CreateStudyGuidePage() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [subject, setSubject] = useState("");

  return (
    <>
      <NavBar />
      <main>
        <h1>Create Study Guide</h1>
        <p>Create a new study guide to organize your learning.</p>

        <form>
          <TextInput
            id="title"
            label="Title"
            value={title}
            onChange={setTitle}
          />

          <label htmlFor="description">Description</label>
          <textarea
            id="description"
            name="description"
            value={description}
            maxLength={300}
            onChange={(event) => setDescription(event.target.value)}
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
