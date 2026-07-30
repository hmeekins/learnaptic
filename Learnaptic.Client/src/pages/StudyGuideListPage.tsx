import NavBar from "../components/NavBar"
import StudyGuideCard from "../components/StudyGuideCard"
import { useState } from "react"

type StudyGuideListItem =
{
        id: number,
        title: string,
        subject?: string,
        lastAccessed: string,
}

function StudyGuideListPage() {
    const [studyGuides, setStudyGuides] = useState<StudyGuideListItem[]>([])

    function handleDeleteStudyGuide(idToDelete: number) {
        setStudyGuides(previousStudyGuides => previousStudyGuides.filter(guide => guide.id !== idToDelete))
    }

    return (
        <>
            <NavBar />
            <main>
                <h1>Study Guides</h1>
                <p>Your study materials, all in one place.</p>
                {studyGuides.map(guide => <StudyGuideCard
                    key={guide.id}
                    id={guide.id}
                    title={guide.title}
                    subject={guide.subject}
                    lastAccessed={guide.lastAccessed}
                    onDelete={() => handleDeleteStudyGuide(guide.id)}
                />)}
                <button>Create New Study Guide</button>
            </main>
        </>
    )
}

export default StudyGuideListPage