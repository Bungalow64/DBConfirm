import React from 'react';
import { NavLink, useHistory } from 'react-router-dom';
import Footer from './Footer';
import './Sidebar.scss';

export default function Sidebar() {

    let history = useHistory();

    function handleChange(target) {
        history.push(target.value);
    };

    return (
        <div id="sidebar">
            <div id="sidebar-heading">
                DBConfirm
            </div>
            <nav>
                <ul id="menu">
                    <li><NavLink exact to="/" activeClassName="active">Overview</NavLink></li>
                    <li><NavLink to="/quickstart" activeClassName="active">Quick Start</NavLink></li>
                    <li><NavLink to="/writingtests" activeClassName="active">Writing Tests</NavLink></li>
                    <li><NavLink to="/templates" activeClassName="active">Templates</NavLink></li>
                    <li><NavLink to="/nuget" activeClassName="active">NuGet Packages</NavLink></li>
                    <li><NavLink to="/connectionstrings" activeClassName="active">Connection Strings</NavLink></li>
                    <li><NavLink to="/continuousintegration" activeClassName="active">Continuous Integration</NavLink></li>
                    <li><NavLink to="/manualsetup" activeClassName="active">Manual Setup</NavLink></li>
                    <li><NavLink to="/debugging" activeClassName="active">Debugging</NavLink></li>
                    <li><NavLink to="/walkthrough" activeClassName="active">Walkthrough</NavLink></li>
                    <li><NavLink to="/api" activeClassName="active">API Reference</NavLink></li>
                    <li><NavLink to="/faq" activeClassName="active">FAQ</NavLink></li>
                </ul>
            </nav>
            <Footer />
            <div id="small-menu">
                <label id="menu-select-label" htmlFor="menu-select">Menu <svg className="down-arrow" xmlns="http://www.w3.org/2000/svg" width="10" height="10" viewBox="0 0 24 24"><path d="M0 7.33l2.829-2.83 9.175 9.339 9.167-9.339 2.829 2.83-11.996 12.17z" /></svg></label>
                <select id="menu-select" onChange={event => handleChange(event.target)}>
                    <option value="/">Overview</option>
                    <option value="/quickstart">Quick Start</option>
                    <option value="/writingtests">Writing Tests</option>
                    <option value="/templates">Templates</option>
                    <option value="/nuget">NuGet Packages</option>
                    <option value="/connectionstrings">Connection Strings</option>
                    <option value="/continuousintegration">Continuous Integration</option>
                    <option value="/manualsetup">Manual Setup</option>
                    <option value="/debugging">Debugging</option>
                    <option value="/walkthrough">Walkthrough</option>
                    <option value="/api">API Reference</option>
                    <option value="/faq">FAQ</option>
                </select>
            </div>
        </div>
    )
};