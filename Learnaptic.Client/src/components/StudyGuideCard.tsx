type StudyGuideCardProps = {
    id: number;
    title: string;
    subject?: string;
    lastAccessed: string;
}

function StudyGuideCard(props: StudyGuideCardProps) {
    
    return (
        <div>
            <h2>{props.title}</h2>
            {props.subject && <p>{props.subject}</p>}
            <p>Last Accessed:{props.lastAccessed}</p>
        </div>
    )
}

export default StudyGuideCard