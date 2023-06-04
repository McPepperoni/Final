var dropdownTitle = document.getElementById("dropdown-title");

if (dropdownTitle) {
  dropdownTitle.onclick = () => {
    var parent = dropdownTitle.parentElement;

    var dropdownBody = parent.getElementsByClassName("dropdown-body")[0];

    if (dropdownBody.classList.contains("dropdown-show")) {
      dropdownBody.classList.remove("dropdown-show");
    } else {
      dropdownBody.classList.add("dropdown-show");
    }
  };
}

///
const cartContainer = document.getElementsByClassName("cart-container")[0];
if (cartContainer) {
  var cartItems = Array.from(document.getElementsByClassName("cart-item"));

  var placeOrderButton = document.getElementById("place-order-button");
  var items = cartItems.map((x) => {
    var selected = x.getElementsByClassName("cart-item-input")[0];
    selected.checked = true;
    var quantity = x.getElementsByClassName("item-quantity")[0];
    var totalPrice = x.getElementsByClassName("item-price-total")[0];
    var price = x.getElementsByClassName("item-price")[0];

    return {
      selected,
      quantity,
      price,
      totalPrice,
    };
  });

  var filteredSelect = items.filter((x) => x.selected.checked);
  var cartTotalPrice = document.getElementById("cart-total-price");

  placeOrderButton.innerText = `Place Order (${filteredSelect.length})`;

  const onChangeSelectedCartItems = () => {
    filteredSelect = items.filter((x) => x.selected.checked);
    placeOrderButton.innerText = `Place Order (${filteredSelect.length})`;

    placeOrderButton.disabled = false;
    if (filteredSelect.length == 0) {
      placeOrderButton.disabled = true;
    }

    var newTotal = filteredSelect.map((x) => {
      return Number(x.totalPrice.innerText.replace(/\D/g, ""));
    });

    cartTotalPrice.innerText =
      newTotal.reduce((partialSum, a) => partialSum + a, 0).toLocaleString() +
      " đ";
  };

  const onChangeQuantity = (e, totalPrice, price) => {
    var value = e.target.value;
    var numberedPrice = Number(price.innerText.replace(/\D/g, ""));
    totalPrice.innerText = `${(
      numberedPrice * Number(value)
    ).toLocaleString()} đ`;

    onChangeSelectedCartItems();
  };

  items.forEach((element, _, arr) => {
    element.selected.onclick = () => onChangeSelectedCartItems();
    element.quantity.onchange = (e) =>
      onChangeQuantity(e, element.totalPrice, element.price);
  });
}

const userEditForm = document.getElementById("user-edit-form");

if (userEditForm) {
  const SaveButton = document.getElementById("save-button");
  const Inputs = Array.from(document.getElementsByTagName("input"));

  const initValue = Inputs.map((x) => x.value + x.checked).join("");
  const onChangeInput = () => {
    console.log(initValue);
    if (Inputs.filter((x) => x.innerText).join("") != initValue) {
      SaveButton.disabled = false;
      SaveButton.innerText = "Save *";
      return;
    }
    SaveButton.disabled = true;
    SaveButton.innerText = "Save";
  };

  Inputs.forEach((x) => {
    x.onchange = onChangeInput;
  });
}
