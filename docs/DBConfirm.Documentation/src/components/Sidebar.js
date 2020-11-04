import React from 'react';
import Menu from './Menu';
import Footer from './Footer';
import './Sidebar.scss';

export default function Sidebar() {
    return (
        <div id="sidebar">
            <div id="sidebar-heading">
                DBConfirm
            </div>
            <Menu />
            <Footer />
        </div>
    )
};