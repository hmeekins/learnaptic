import formatTimeElapsed from "../utils/formatDate";

type StudyGuideCardProps = {
  id: number;
  title: string;
  subject?: string;
  lastAccessedAt: string;
  onDelete: () => void;
  onClick: () => void;
};

function StudyGuideCard(props: StudyGuideCardProps) {
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

export default StudyGuideCard;
