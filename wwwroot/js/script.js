// Custom JavaScript for Dental Service website

// Initialize Bootstrap components
document.addEventListener('DOMContentLoaded', function() {
    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });

    // Tab functionality
    const tabButtons = document.querySelectorAll('.btn-tab');

    tabButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Remove active class from all buttons
            tabButtons.forEach(btn => {
                btn.classList.remove('active');
            });

            // Add active class to clicked button
            this.classList.add('active');
        });
    });

    // Time slot selection
    const timeButtons = document.querySelectorAll('.btn-outline-secondary');

    timeButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Toggle active class
            timeButtons.forEach(btn => {
                btn.classList.remove('active');
            });
            this.classList.add('active');
        }
        );
    }
    );
    // Initialize Bootstrap tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    const tooltipList = tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    // Initialize Bootstrap popovers
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    const popoverList = popoverTriggerList.map(function(popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
    // Initialize Bootstrap modals
    const modalTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="modal"]'));
    const modalList = modalTriggerList.map(function(modalTriggerEl) {
        return new bootstrap.Modal(modalTriggerEl);
    });
    // Initialize Bootstrap carousels
    const carouselElementList = [].slice.call(document.querySelectorAll('.carousel'));
    const carouselList = carouselElementList.map(function(carouselElement) {
        return new bootstrap.Carousel(carouselElement);
    });
    // Initialize Bootstrap dropdowns
    const dropdownElementList = [].slice.call(document.querySelectorAll('.dropdown-toggle'));
    const dropdownList = dropdownElementList.map(function(dropdownElement) {
        return new bootstrap.Dropdown(dropdownElement);
    });

    // Force re-initialize dropdowns after page load to ensure they work
    setTimeout(function() {
        const navbarDropdown = document.getElementById('navbarDropdown');
        if (navbarDropdown && !navbarDropdown.hasAttribute('data-bs-dropdown-initialized')) {
            new bootstrap.Dropdown(navbarDropdown);
            navbarDropdown.setAttribute('data-bs-dropdown-initialized', 'true');
        }
    }, 100);

});

// DataTable initialization
// Note: DataTables are initialized in their specific view files
// to avoid conflicts and allow for custom configuration per page:
// - patient-table: initialized in Patient/Index.cshtml
// - userTable: initialized in UserManagement/Index.cshtml
// - appointmentTable: initialized in Payment/Index.cshtml
// - activityTable: initialized in Home/Index.cshtml

// Patient Management Functions
function savePatient() {
    var formData = $('#addPatientForm').serialize();
    $.ajax({
        url: '/Admin/CreatePatient',
        type: 'POST',
        data: formData,
        success: function(response) {
            if (response.success) {
                $('#addPatientModal').modal('hide');
                location.reload();
            } else {
                alert(response.errors.join('\n'));
            }
        },
        error: function() {
            alert('Có lỗi xảy ra khi lưu bệnh nhân');
        }
    });
}

function editPatient(id) {
    $.get('/Admin/GetPatient/' + id, function(patient) {
        $('#editPatientId').val(patient.id);
        $('#editFullName').val(patient.fullName);
        $('#editEmail').val(patient.email);
        $('#editPhoneNumber').val(patient.phoneNumber);
        $('#editDateOfBirth').val(patient.dateOfBirth);
        $('#editGender').val(patient.gender);
        $('#editAddress').val(patient.address);
        $('#editPatientModal').modal('show');
    });
}

function updatePatient() {
    var formData = $('#editPatientForm').serialize();
    $.ajax({
        url: '/Admin/UpdatePatient',
        type: 'POST',
        data: formData,
        success: function(response) {
            if (response.success) {
                $('#editPatientModal').modal('hide');
                location.reload();
            } else {
                alert(response.errors.join('\n'));
            }
        },
        error: function() {
            alert('Có lỗi xảy ra khi cập nhật bệnh nhân');
        }
    });
}

function viewDetails(id) {
    $.get('/Admin/GetPatient/' + id, function(patient) {
        $('#viewFullName').text(patient.fullName);
        $('#viewEmail').text(patient.email);
        $('#viewPhoneNumber').text(patient.phoneNumber);
        $('#viewDateOfBirth').text(new Date(patient.dateOfBirth).toLocaleDateString());
        $('#viewGender').text(patient.gender);
        $('#viewAddress').text(patient.address);
        $('#viewPatientModal').modal('show');
    });
}

function deletePatient(id) {
    if (confirm('Bạn có chắc chắn muốn xóa bệnh nhân này?')) {
        $.ajax({
            url: '/Admin/DeletePatient',
            type: 'POST',
            data: { id: id },
            success: function(response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function() {
                alert('Có lỗi xảy ra khi xóa bệnh nhân');
            }
        });
    }
}

// User Management Functions
function toggleUserStatus(id) {
    $.ajax({
        url: '/Admin/ToggleUserStatus',
        type: 'POST',
        data: { id: id },
        success: function(response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function() {
            alert('Có lỗi xảy ra khi cập nhật trạng thái người dùng');
        }
    });
}

function deleteUser(id) {
    if (confirm('Bạn có chắc chắn muốn xóa người dùng này?')) {
        $.ajax({
            url: '/Admin/DeleteUser',
            type: 'POST',
            data: { id: id },
            success: function(response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function() {
                alert('Có lỗi xảy ra khi xóa người dùng');
            }
        });
    }
}

// Appointment Management Functions
function saveAppointment() {
    var formData = $('#addAppointmentForm').serialize();
    $.ajax({
        url: '/Admin/CreateAppointment',
        type: 'POST',
        data: formData,
        success: function(response) {
            if (response.success) {
                $('#addAppointmentModal').modal('hide');
                location.reload();
            } else {
                alert(response.errors.join('\n'));
            }
        },
        error: function() {
            alert('Có lỗi xảy ra khi lưu lịch hẹn');
        }
    });
}

function updateAppointment() {
    var formData = $('#editAppointmentForm').serialize();
    $.ajax({
        url: '/Admin/UpdateAppointment',
        type: 'POST',
        data: formData,
        success: function(response) {
            if (response.success) {
                $('#editAppointmentModal').modal('hide');
                location.reload();
            } else {
                alert(response.errors.join('\n'));
            }
        },
        error: function() {
            alert('Có lỗi xảy ra khi cập nhật lịch hẹn');
        }
    });
}

function deleteAppointment(id) {
    if (confirm('Bạn có chắc chắn muốn xóa lịch hẹn này?')) {
        $.ajax({
            url: '/Admin/DeleteAppointment',
            type: 'POST',
            data: { id: id },
            success: function(response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function() {
                alert('Có lỗi xảy ra khi xóa lịch hẹn');
            }
        });
    }
}

// Service Management Functions
function saveService() {
    var formData = $('#addServiceForm').serialize();
    $.ajax({
        url: '/Admin/CreateService',
        type: 'POST',
        data: formData,
        success: function(response) {
            if (response.success) {
                $('#addServiceModal').modal('hide');
                location.reload();
            } else {
                alert(response.errors.join('\n'));
            }
        },
        error: function() {
            alert('Có lỗi xảy ra khi lưu dịch vụ');
        }
    });
}

function updateService() {
    var formData = $('#editServiceForm').serialize();
    $.ajax({
        url: '/Admin/UpdateService',
        type: 'POST',
        data: formData,
        success: function(response) {
            if (response.success) {
                $('#editServiceModal').modal('hide');
                location.reload();
            } else {
                alert(response.errors.join('\n'));
            }
        },
        error: function() {
            alert('Có lỗi xảy ra khi cập nhật dịch vụ');
        }
    });
}

function deleteService(id) {
    if (confirm('Bạn có chắc chắn muốn xóa dịch vụ này?')) {
        $.ajax({
            url: '/Admin/DeleteService',
            type: 'POST',
            data: { id: id },
            success: function(response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function() {
                alert('Có lỗi xảy ra khi xóa dịch vụ');
            }
        });
    }
}
