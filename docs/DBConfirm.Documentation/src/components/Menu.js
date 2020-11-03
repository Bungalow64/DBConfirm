import React from 'react';
import { NavLink } from 'react-router-dom';

export default function Menu() {
    return (
        <nav>
            <ul id="menu">
                <li><NavLink exact to="/" activeClassName="active">Overview</NavLink></li>
                <li><NavLink to="/quickstart" activeClassName="active">Quick Start</NavLink></li>
                <li><NavLink to="/writingtests" activeClassName="active">Writing Tests</NavLink></li>
                <li><NavLink to="/templates" activeClassName="active">Templates</NavLink></li>
                <li><NavLink to="/nuget" activeClassName="active">NuGet Packages</NavLink></li>
                <li><NavLink to="/connectionstrings" activeClassName="active">Connection Strings</NavLink></li>
                <li><NavLink to="/continuousintegration" activeClassName="active">Continuous Integration</NavLink></li>
                <li><NavLink to="/debugging" activeClassName="active">Debugging</NavLink></li>
                <li><NavLink to="/api" activeClassName="active">API Reference</NavLink></li>
                <li><NavLink to="/faq" activeClassName="active">FAQ</NavLink></li>
            </ul>
        </nav>
    )
};