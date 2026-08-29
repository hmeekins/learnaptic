import { type NotebookColor } from "@/constants/notebookColors";
import NotebookIcon from "./NotebookIcon";
import formatTimeElapsed from "@/utils/formatDate";

interface NotebookCardProps {
  id: number;
  title: string;
  subject?: string;
  color: NotebookColor;
  lastAccessedAt: string;
  onDelete: () => void;
  onClick: () => void;
}

function NotebookCard(props: NotebookCardProps) {
  const dateString = formatTimeElapsed(props.lastAccessedAt);

  return (
    <div
      onClick={props.onClick}
      className="flex cursor-pointer items-center gap-4 rounded-xl border bg-card p-4 transition-colors hover:bg-muted/50"
    >
      <div className="h-16 w-14 shrink-0">
        <NotebookIcon color={props.color} />
      </div>

      <div className="min-w-0 flex-1">
        <h2 className="truncate font-semibold">{props.title}</h2>

        {props.subject && (
          <p className="text-sm text-muted-foreground">{props.subject}</p>
        )}

        <p className="mt-1 text-xs text-muted-foreground">
          Last accessed {dateString}
        </p>
      </div>

      <button
        onClick={(event) => {
          event.stopPropagation();
          props.onDelete();
        }}
      >
        Delete
      </button>
    </div>
  );
}

export default NotebookCard;
