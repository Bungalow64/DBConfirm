import React, { useRef } from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import Sidebar from './components/Sidebar';
import Overview from './pages/Overview';
import WritingTests from './pages/WritingTests';
import QuickStart from './pages/QuickStart';
import NuGet from './pages/NuGet';
import Templates from './pages/Templates';
import ConnectionStrings from './pages/ConnectionStrings';
import ContinuousIntegration from './pages/ContinuousIntegration';
import ManualSetup from './pages/ManualSetup';
import Debugging from './pages/Debugging'
import Walkthrough from './pages/Walkthrough';
import Api from './pages/Api'
import FAQ from './pages/FAQ';
import ScrollToTop from './components/ScrollToTop';
import './App.scss';
import Footer from './components/Footer';

function App() {

  let contentRef = useRef();

  return (
    <Router>
      <Sidebar />
      <div id="content" ref={contentRef}>
        <ScrollToTop scrollContent={contentRef} />
        <h1>DBConfirm - Official Documentation</h1>
        <Switch>
          <Route path="/quickstart"><QuickStart /></Route>
          <Route path="/writingtests"><WritingTests /></Route>
          <Route path="/nuget"><NuGet /></Route>
          <Route path="/templates"><Templates /></Route>
          <Route path="/connectionstrings"><ConnectionStrings /></Route>
          <Route path="/continuousintegration"><ContinuousIntegration /></Route>
          <Route path="/manualsetup"><ManualSetup /></Route>
          <Route path="/debugging"><Debugging /></Route>
          <Route path="/api"><Api /></Route>
          <Route path="/faq"><FAQ /></Route>
          <Route path="/walkthrough"><Walkthrough /></Route>
          <Route path="/"><Overview /></Route>
        </Switch>
        <Footer />
      </div>
    </Router>
  );
}

export default App;
