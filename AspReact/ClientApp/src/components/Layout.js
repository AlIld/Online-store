import React, {Component} from 'react';
import {Col, Container, Row} from 'reactstrap';
import {NavMenu} from './NavMenu';
import {CategoryLayout} from "./CategoryLayout";
import "./CategoryLayout.css"

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <NavMenu/>
        <Container fluid>
          <Row>
            <Col sm md="2" id="sidebar-wrapper">
              <CategoryLayout/>
            </Col>
            <Col sm md="10" id="page-content-wrapper">
              {this.props.children}
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}
