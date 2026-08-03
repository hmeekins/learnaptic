import NavBar from "../components/NavBar";
import ConceptTableOfContents from "../components/ConceptTableOfContents";

function ViewStudyGuidePage() {
  return (
    <>
      <NavBar />
      <main>
        <header>
          <h1>Biology Exam Review</h1>
          <p>Biology • Updated yesterday</p>
        </header>

        <ConceptTableOfContents concepts={[]} />

        <section>
          <p>Concept content goes here.</p>
        </section>
      </main>
    </>
  );
}

export default ViewStudyGuidePage;
