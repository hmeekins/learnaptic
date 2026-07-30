type StudyGuideCardProps = {
    id: number
    title: string
    subject?: string
    lastAccessedAt: string
    onDelete: () => void
}

function StudyGuideCard(props: StudyGuideCardProps) {
    return (
        <div>
            <h2>{props.title}</h2>
            {props.subject && <p>{props.subject}</p>}
            <p>Last Accessed: {props.lastAccessedAt}</p>
            <button onClick={props.onDelete}>Delete Study Guide</button>
        </div>
    )
}

export default StudyGuideCard