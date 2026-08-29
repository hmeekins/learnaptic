import formatTimeElapsed from "@/utils/formatDate";
import { type NotebookColor } from "@/constants/notebookColors";

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
    <div onClick={props.onClick}>
      <h2>{props.title}</h2>
      {props.subject && <p>{props.subject}</p>}
      <p>Last Accessed: {dateString}</p>
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
