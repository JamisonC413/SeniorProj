import React from 'react';
import { Link } from "react-router-dom";
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import 'bootstrap/dist/css/bootstrap.min.css';

function NavBar() {
  return (
    <Navbar expand="lg" className="bg-body-tertiary" data-bs-theme="dark">
      <Container>
        <Navbar.Brand href="/">Algorithmic Art</Navbar.Brand>
        <Nav className="me-auto">
            <Nav.Link href="/imageGallery">Image Gallery</Nav.Link>
            <Nav.Link href="/">Tutorials</Nav.Link>
        </Nav>
    </Container>
  </Navbar>

  );
}

export default NavBar;