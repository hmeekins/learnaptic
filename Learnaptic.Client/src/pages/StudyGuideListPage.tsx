import NavBar from "../components/NavBar"
import StudyGuideCard from "../components/StudyGuideCard"
import { useEffect, useState } from "react"

type StudyGuideListItem =
    {
        id: number,
        title: string,
        subject?: string,
        lastAccessedAt: string,
    }

function StudyGuideListPage() {
    const [studyGuides, setStudyGuides] = useState<StudyGuideListItem[]>([])

    useEffect(
        () => {
            async function loadStudyGuides() {
                const response = await fetch("https://localhost:7057/api/study-guides")
                console.log("Status:", response.status)
                console.log("OK:", response.ok)
                const data = await response.json()

                console.log("Data:", data)
                setStudyGuides(data)
                
            }

            loadStudyGuides()
        },
        []
    )

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
                    lastAccessedAt={guide.lastAccessedAt}
                    onDelete={() => handleDeleteStudyGuide(guide.id)}
                />)}
                <button>Create New Study Guide</button>
            </main>
        </>
    )
}

export default StudyGuideListPage