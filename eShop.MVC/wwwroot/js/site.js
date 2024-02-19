document.getElementById('clearFilterBtn').addEventListener('click', function () {
    document.getElementById('minPrice').value = 0
    document.getElementById('maxPrice').value = 0
    document.getElementById('typeSelect').value = ''
    document.getElementById('brandSelect').value = ''
})


window.onscroll = function () { scrollFunction() };
function scrollFunction() {
    if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
        document.getElementById('myBtn').style.display = 'block'
        return
    }
    document.getElementById('myBtn').style.display = 'none'
}
function topFunction() {
    document.body.scrollTop = 0
    document.documentElement.scrollTop = 0
}