import $ from 'jquery';
import jquery_validate from 'jquery-validation';
import jquery_validate_unobtrusive from 'jquery-validation-unobtrusive';
import lib from './lib';

import React from 'react';
import ReactDOM from 'react-dom';
import Counter from './reactcomponent';

ReactDOM.render(
    <Counter />,
    document.getElementById('basicreactcomponent')
);

require('./lib');
document.getElementById("fillthis").innerHTML = getText();
$('#fillthiswithjquery').html('Filled by Jquery!');

module.hot.accept();
console.info("Probando esta todo ok");