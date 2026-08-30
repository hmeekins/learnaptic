import { notebookColors, type NotebookColor } from "@/constants/notebookColors";
import type { CreateNotebookRequest } from "@/types/Notebooks/CreateNotebookRequest";
import { useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import NotebookIcon from "@/components/notebooks/NotebookIcon";

interface NotebookFormProps {
  initialTitle?: string;
  initialSubject?: string;
  initialColor?: NotebookColor;
  submitLabel: string;
  isSubmitting: boolean;
  onSubmit: (data: CreateNotebookRequest) => Promise<string | null>;
}

function NotebookForm(props: NotebookFormProps) {
  const [title, setTitle] = useState(props.initialTitle ?? "");
  const [subject, setSubject] = useState(props.initialSubject ?? "");
  const [color, setColor] = useState<NotebookColor>(
    props.initialColor ?? "teal"
  );
  const [error, setError] = useState("");

  async function handleSubmit(event: React.SubmitEvent<HTMLFormElement>) {
    event.preventDefault();
    if (props.isSubmitting) return;
    setError("");

    if (title.trim().length < 3) {
      setError("Title must be at least 3 characters");
      return;
    }

    const notebookRequest: CreateNotebookRequest = {
      title: title.trim(),
      color,
    };

    if (subject.trim() !== "") {
      notebookRequest.subject = subject.trim();
    }

    const response = await props.onSubmit(notebookRequest);

    if (response != null) setError(response);
  }

  return (
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
        <Button type="submit" disabled={props.isSubmitting}>
          {props.isSubmitting ? "Saving..." : props.submitLabel}
        </Button>
      </div>

      <div className="flex justify-center">
        {error && <p className="text-sm text-destructive">{error}</p>}
      </div>
    </form>
  );
}

export default NotebookForm;
