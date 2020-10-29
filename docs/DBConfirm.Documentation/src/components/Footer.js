import React from 'react';
import CurrentDate from '../logic/CurrentDate.js';

export default function Footer() {
    return (
        <footer>&copy; {CurrentDate.now().getFullYear()}<br/>Bungalow64 Technologies Ltd.</footer>
    )
};