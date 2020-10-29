import React, { useRef } from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import Sidebar from './components/Sidebar';
import Overview from './pages/Overview';
import QuickStart from './pages/QuickStart';
import NuGet from './pages/NuGet';
import Templates from './pages/Templates';
import ContinuousIntegration from './pages/ContinuousIntegration';
import Api from './pages/Api'
import ScrollToTop from './components/ScrollToTop';
import './App.scss';

function App() {

  let contentRef = useRef();

  return (
    <Router>
      <Sidebar/>
      <div id="content" ref={contentRef}>
        <ScrollToTop scrollContent={contentRef}/>
          <h1>DBConfirm - Official Documentation</h1>
          <Switch>
            <Route path="/quickstart"><QuickStart /></Route>
            <Route path="/nuget"><NuGet /></Route>
            <Route path="/templates"><Templates /></Route>
            <Route path="/continuousintegration"><ContinuousIntegration /></Route>
            <Route path="/api"><Api /></Route>
            <Route path="/"><Overview /></Route>
          </Switch>
      </div>
    </Router>
  );
}

export default App;