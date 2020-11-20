import React from 'react';
import CurrentDate from '../logic/CurrentDate.js';

export default function Footer() {
    return (
        <footer><span>&copy; {CurrentDate.now().getFullYear()}</span><span>Bungalow64 Technologies Ltd.</span></footer>
    )
};