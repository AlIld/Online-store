import React, {Component} from 'react';
import {ProductList} from "./ProductList";
import * as signalR from "@microsoft/signalr";
import authService from "./api-authorization/AuthorizeService";

export class Chat extends Component {
  static displayName = Chat.name;

  constructor(props) {
    super(props);

    this.state = {
      isAuthenticated: false,
      userName: null
    };
  }

  async componentDidMount() {
    await this.populateState();
    setTimeout(this.connect.bind(this));
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }

  render() {
    return (
      <div className="sticky-top h-50">
        <h4>Chat:</h4>
        <div id="divMessages" className="messages overflow-auto">
        </div>
        {
          this.state.isAuthenticated
            ?
            <div className="input-zone sticky-top">
              <h5>
                <label id="lblMessage" htmlFor="tbMessage">Your Message:</label>
              </h5>
              <input id="tbMessage" className="input-zone-input" type="text"/>
              <br/>
              <button id="btnSend">Send</button>
            </div>
            : null
        }

      </div>
    );
  }

  async populateState() {
    const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()]);
    this.setState({
      isAuthenticated,
      userName: user && user.name
    });
  }

  async connect() {
    const divMessages = document.querySelector("#divMessages");
    const tbMessage = document.querySelector("#tbMessage");
    const btnSend = document.querySelector("#btnSend");
    const username = new Date().getTime();

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("/api/chat")
      .build();

    connection.on("messageReceived", (username, message) => {
      let m = document.createElement("div");
      m.className = "sticky-top";

      m.innerHTML =
        `<h6 class="message-author">${username}:</h6><div>${message}</div>`;

      divMessages.appendChild(m);
      divMessages.scrollTop = divMessages.scrollHeight;
    });

    connection.start().catch(err => document.write(err));

    if (this.state.isAuthenticated) {
      tbMessage.addEventListener("keyup", (e) => {
        if (e.key === "Enter") {
          send.bind(this)();
        }
      });
      
      btnSend.addEventListener("click", send.bind(this));
    }

    function send() {
      connection.send("newMessage", this.state.userName, tbMessage.value)
        .then(() => tbMessage.value = "");
    }
  }
}
