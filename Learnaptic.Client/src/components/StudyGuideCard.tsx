type StudyGuideCardProps = {
    id: number;
    title: string;
    description?: string;
    subject?: string;
    lastAccessed: string;
}

function StudyGuideCard(props: StudyGuideCardProps) {
    return <h2>{props.title}</h2>
}

export default StudyGuideCard