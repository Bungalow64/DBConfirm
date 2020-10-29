import React from 'react';
import { BrowserRouter as Router, Switch, Route, NavLink } from 'react-router-dom';
import Menu from './components/Menu';
import Footer from './components/Footer';
import Overview from './pages/Overview';
import QuickStart from './pages/QuickStart';
import NuGet from './pages/NuGet';
import Templates from './pages/Templates';
import ContinuousIntegration from './pages/ContinuousIntegration';
import Api from './pages/Api'
import ScrollToTop from './ScrollToTop';
import './App.scss';

function App() {
  return (
    <Router>
      <div id="sidebar">
        <div id="sidebar-heading">
          DBConfirm
        </div>
        <Menu/>
        <Footer/>
      </div>
      <div id="content">
        <ScrollToTop/>
          <h1>DBConfirm - Documentation</h1>
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
