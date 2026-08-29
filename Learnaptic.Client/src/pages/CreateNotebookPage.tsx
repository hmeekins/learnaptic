import { notebookColors, type NotebookColor } from "@/constants/notebookColors";
import type { CreateNotebookRequest } from "../types/Notebooks/CreateNotebookRequest";
import type { NotebookDetail } from "../types/Notebooks/NotebookDetail";
import { useState } from "react";
import { useNavigate } from "react-router";
import { API_URL } from "@/config/api";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import NotebookIcon from "@/components/notebooks/NotebookIcon";

function CreateNotebookPage() {
  const [title, setTitle] = useState("");
  const [subject, setSubject] = useState("");
  const [color, setColor] = useState<NotebookColor>("teal");

  const navigate = useNavigate();

  async function handleSubmit(event: React.SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
    if (title.trim() === "") {
      return;
    }

    const notebookRequest: CreateNotebookRequest = {
      title: title.trim(),
      color: color,
    };
    if (subject.trim() !== "") {
      notebookRequest.subject = subject.trim();
    }
    const response = await fetch(`${API_URL}/api/notebooks`, {
      credentials: "include",
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
      <main className="mx-auto max-w-6xl px-6 py-8">
        <div className="mx-auto max-w-xl">
          <div className="space-y-1">
            <h1 className="text-xl font-bold">Create Notebook</h1>

            <p className="text-sm text-muted-foreground">
              Create a new notebook to organize your learning.
            </p>
          </div>

          <form onSubmit={handleSubmit} className="mt-4 max-w-xl space-y-4">
            <Input
              id="title"
              placeholder="Title"
              value={title}
              onChange={(t) => setTitle(t.target.value)}
            />

            <Input
              id="subject"
              placeholder="Subject"
              value={subject}
              onChange={(s) => setSubject(s.target.value)}
            />

            <div>
              <div className="grid grid-cols-4 gap-3">
                {Object.keys(notebookColors).map((colorOption) => {
                  const notebookColor = colorOption as NotebookColor;

                  return (
                    <button
                      key={notebookColor}
                      type="button"
                      onClick={() => setColor(notebookColor)}
                      className={`rounded-lg border-2 p-2 ${
                        color === notebookColor
                          ? "border-primary"
                          : "border-transparent"
                      }`}
                    >
                      <div className="mx-auto h-16 w-14">
                        <NotebookIcon color={notebookColor} />
                      </div>
                    </button>
                  );
                })}
              </div>
            </div>

            <div className="flex justify-center">
              <Button type="submit">Create Notebook</Button>
            </div>
          </form>
        </div>
      </main>
    </>
  );
}

export default CreateNotebookPage;
