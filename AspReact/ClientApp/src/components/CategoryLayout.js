import React, {Component} from 'react';
import {CardColumns, Navbar, NavItem, NavLink} from "reactstrap";
import "./CategoryLayout.css"
import {Product} from "./Product";
import {Link} from "react-router-dom";
import {Chat} from "./Chat";

export class CategoryLayout extends Component {
  static displayName = CategoryLayout.name;

  constructor(props) {
    super(props);
    this.state = {categories: [], loading: true};
  }

  componentDidMount() {
    this.populateCategories();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      :
      this.state.categories.map(item =>
        <NavLink tag={Link} to={"/categoryProducts/" + item.id}>{item.name}</NavLink>);

    return (
      <Navbar className="col-md-12 d-none d-md-block bg-light sidebar">
        <h4>Categories</h4>
        {contents}
        <Chat/>
      </Navbar>
    );
  }

  async populateCategories() {
    const response = await fetch('api/category/index');
    const data = await response.json();
    this.setState({categories: data, loading: false});
  }
}